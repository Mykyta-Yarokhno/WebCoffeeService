using CoffeeMachineService.Models;

namespace CoffeeMachineService
{
    public class MachineStatus 
    {
        public MachineState CurrentMachineState { get; private set; } = MachineState.Off;

        private object _syncObj = new();

        public MachineState GetCurrentMachineState(TimeSpan delay)
        {
            Thread.Sleep(delay);
            return CurrentMachineState;
        }

        public void TurnOn()
        {
            if (CurrentMachineState != MachineState.Off)
            {
                return;
            } 
            lock (_syncObj)
            {
                if (CurrentMachineState != MachineState.Off)
                {
                    return;
                }
                CurrentMachineState = MachineState.TurnsOn;
            }

            Task.Delay((int)TimeSpan.FromSeconds(20).TotalMilliseconds);
            CurrentMachineState = MachineState.On;
        }

        public void TurnOff()
        {
            if (CurrentMachineState != MachineState.On)
            {
                return;
            }
            lock (_syncObj)
            {
                if (CurrentMachineState != MachineState.On)
                {
                    return;
                }
                CurrentMachineState = MachineState.TurnsOff;
            }
            Task.Delay(20000);
            CurrentMachineState = MachineState.Off;
        }

        
    }
}
