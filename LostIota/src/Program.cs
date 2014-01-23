using System;

namespace LostIota
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (LostIota game = new LostIota())
            {
                game.Run();
            }
        }
    }
#endif
}

