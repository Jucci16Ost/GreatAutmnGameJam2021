using Assets.Scripts.DayNightCycle;

namespace Assets.Scripts.Spawners
{
    public class ZombieSpawner : CreatureSpawner
    {
        /// <inheritdoc />
        protected override bool CanSpawn()
        {
            var timeOfDay = DayNightCycleContext.TimeOfDay;
            return timeOfDay < 7 || timeOfDay > 17;
        }
    }
}