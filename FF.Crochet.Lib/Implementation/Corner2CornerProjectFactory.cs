using Autofac;
using FF.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FF.Corner2Corner.Lib
{
    public static class Corner2CornerProjectFactory
    {
        private class CommandFactory : ICommandFactory<Corner2CornerCommand>
        {
            private IContainer container;
            public CommandFactory(ICorner2CornerCommandsInput commandsInput, Corner2CornerProject project, IUndoRedoManager undoRedoManager)
            {
                ContainerBuilder builder = new ContainerBuilder();
                builder.RegisterInstance(commandsInput).As<ICorner2CornerCommandsInput>();
                builder.RegisterInstance(project).As<Corner2CornerProject>();
                builder.RegisterInstance(undoRedoManager).As<IUndoRedoManager>();

                var commands = CommandDiscovery.FindCommands<Corner2CornerCommandAttribute, Corner2CornerCommand>(Assembly.GetExecutingAssembly());

                foreach (var command in commands)
                {
                    builder.RegisterType(command.Value).Keyed<ICommand>(command.Key);
                }

                container = builder.Build();
            }

            public ICommand GetCommand(Corner2CornerCommand commandType)
            {
                return container.ResolveKeyed<ICommand>(commandType);
            }
        }

        public static ICorner2CornerProject Create(ICorner2CornerCommandsInput commandsInput, IUndoRedoManager undoRedoManager)
        {
            Corner2CornerProject result = new Corner2CornerProject();
            result.Commands = new CommandFactory(commandsInput, result, undoRedoManager);
            result.ChangeTracking.Track(ChangeTrackingOperation.SetNew);
            return result;
        }

    }
}
