using UnityEngine;
using UnityEngine.Scripting;

namespace Scellecs.Morpeh
{
    [Preserve]
    public sealed class EventWorldPlugin : IWorldPlugin
    {
        [Preserve]
        public void Initialize(World world)
        {
            world.CodeWriterEventsRegistry = new EventRegistry();
            world.CodeWriterRequestsRegistry = new RequestRegistry();

            var eventSystemGroup = world.CreateSystemsGroup();
            eventSystemGroup.AddSystem(new ProcessEventsSystem(world.CodeWriterEventsRegistry));
            world.AddPluginSystemsGroup(eventSystemGroup);
        }

        public void Deinitialize(World world)
        {
        }
    }
}