using System.Text;
using System.Diagnostics;
using UnityEngine;
using Unity.MLAgentsExamples;

namespace CommandTerminal
{
    public static class BuiltinCommands
    {
        [RegisterCommand(Help = "Spawns a prefab by name", MinArgCount = 1, MaxArgCount = 2)]
        static void CommandPrefabSpawner(CommandArg[] args)
        {
            var spawner = Object.FindFirstObjectByType<PrefabSpawner>();
            if (spawner != null)
            {
                string mode = args[0].String.ToLower();
                switch (mode)
                {
                    case "list":
                        Terminal.Log("Listing prefabs...");
                        foreach (var name in spawner.ListPrefabs())
                        {
                            Terminal.Log(name);
                        }
                        break;
                    case "spawn":
                        Terminal.Log("Spawning prefab...");
                        //string prefabName = args[1].String;
                        spawner.m_XCount = 1;
                        spawner.m_ZCount = 1;
                        spawner.m_WidthX = 20f;
                        spawner.m_WidthZ = 20f;
                        spawner.m_OffsetX = -10f;
                        spawner.m_OffsetZ = -10f;
                        spawner.SpawnSingle();
                        break;
                    default:
                        Terminal.Shell.IssueErrorMessage($"Unknown mode '{mode}' for the Model command.");
                        break;
                }
            }
            else
            {
                Terminal.Log("PrefabSpawner component not found in the scene.");
            }
        }

        [RegisterCommand(Help = "Lists models or copies a model from one path to another", MinArgCount = 1, MaxArgCount = 3)]
        static void CommandModel(CommandArg[] args)
        {
            string mode = args[0].String.ToLower();

            switch (mode)
            {
                case "list":
                    // TODO: Implement listing logic here
                    Terminal.Log("Listing all models...");
                    break;
                default:
                    Terminal.Shell.IssueErrorMessage($"Unknown mode '{mode}' for the Model command.");
                    break;
            }
        }

        [RegisterCommand(Help = "Clears the Command Console", MaxArgCount = 0)]
        static void CommandClear(CommandArg[] args)
        {
            Terminal.Buffer.Clear();
        }

        [RegisterCommand(Help = "Lists all Commands or displays help documentation of a Command", MaxArgCount = 1)]
        static void CommandHelp(CommandArg[] args)
        {
            if (args.Length == 0)
            {
                foreach (var command in Terminal.Shell.Commands)
                {
                    Terminal.Log("{0}: {1}", command.Key.PadRight(16), command.Value.help);
                }
                return;
            }

            string command_name = args[0].String.ToUpper();

            if (!Terminal.Shell.Commands.ContainsKey(command_name))
            {
                Terminal.Shell.IssueErrorMessage("Command {0} could not be found.", command_name);
                return;
            }

            string help = Terminal.Shell.Commands[command_name].help;

            if (help == null)
            {
                Terminal.Log("{0} does not provide any help documentation.", command_name);
            }
            else
            {
                Terminal.Log(help);
            }
        }

        [RegisterCommand(Help = "Outputs message")]
        static void CommandPrint(CommandArg[] args)
        {
            Terminal.Log(JoinArguments(args));
        }

        [RegisterCommand(Help = "Quits running Application", MaxArgCount = 0)]
        static void CommandQuit(CommandArg[] args)
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        static string JoinArguments(CommandArg[] args)
        {
            var sb = new StringBuilder();
            int arg_length = args.Length;

            for (int i = 0; i < arg_length; i++)
            {
                sb.Append(args[i].String);

                if (i < arg_length - 1)
                {
                    sb.Append(" ");
                }
            }

            return sb.ToString();
        }
    }
}
