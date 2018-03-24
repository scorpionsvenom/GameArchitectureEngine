using System;

namespace GameArchitectureEngine
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (ActionRPG game = new ActionRPG())
            {
                game.Run();
            }
        }
    }
#endif
}

