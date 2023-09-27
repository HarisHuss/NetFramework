using Raylib_CsLo;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace SpaceInvaders
{
    class Program
    {
        static void Main(string[] args)
        {
            invaders game = new invaders();
            game.GameLoop();
        }
    }
}
