using ood_project1.Commands;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ood_project1
{
    public class CommandParser
    {
        private Dictionary<string, Func<string, string, List<string>, ICommand>> commandsMap;
        private Data data;

        public CommandParser(Data data)
        {
            this.commandsMap = new Dictionary<string, Func<string, string, List<string>, ICommand>>() { { "delete", (type, conditions, fileds) => {return new DeleteCommand(type, conditions, fileds); } },
                                                                { "update", (type, conditions, fileds) => {return new UpdateCommand(type, conditions, fileds); }},
                                                                { "add", (type, conditions, fileds) => {return new AddCommand(type, conditions, fileds); } },
                                                                { "display", (type, conditions, fileds) => {return new DisplayCommand(type, conditions, fileds); } }};
            this.data = data;
        }
        public void InvokeCommand(string command)
        {
            string[] parts = command.Split(new char[] { ' ', ',', '(', ')' }, StringSplitOptions.RemoveEmptyEntries);
            string commandName = parts[0].ToLower();
            string className = FindClassName(parts, commandName);

            int whereIndex = command.IndexOf("where");
            string conditions = "";
            if (whereIndex != -1)
            {
                int startIndex = whereIndex + "where".Length;
                conditions = command.Substring(startIndex).Trim();
            }

            List<string> fields = new List<string>();

            for (int i = 1; i < parts.Length && (parts[i] != "from" && parts[i] != "set"); i++)
            {
                fields.Add(parts[i]);
            }


            if (commandsMap.ContainsKey(commandName))
            {
                var commandObject = commandsMap[commandName].Invoke(className, conditions, fields);
                commandObject.ExecuteCommand(className, command, data);
            }
            else
            {
                throw new ArgumentException("Invalid command type");
            }
        }
        private string FindClassName(string[] parts, string commandName)
        {
            if (commandName == "display")
            {
                int index = Array.IndexOf(parts, "from");
                if (index != -1)
                {
                    return parts[index + 1].ToLower();
                }
                else
                {
                    throw new ArgumentException("Invalid command");
                }
            }
            return parts[1].ToLower();
        }

    }
}
