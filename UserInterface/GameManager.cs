using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using GamePlayers;
using GameLogic;

namespace UserInterface
{
    public class GameManager
    {
        public static void Run()
        {
            MenuForm            settingMenu = new MenuForm();
            settingMenu.ShowDialog();

            while (settingMenu.GameForm.ReRun)
            {
                settingMenu.GameForm.ReRun = false;
                settingMenu.GameForm.Refresh();
                settingMenu.GameForm.ShowDialog();
            }
        }
    }
}
