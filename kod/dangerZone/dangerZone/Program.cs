using System;

namespace dangerZone
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (DangerZone game = new DangerZone())
            {
                game.Run();
            }
        }
    }
#endif
}

