using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerminalScannerPlus
{
    internal class Utilities
    {
        internal static string ScanDisplayText()
        {
            string items = "";
            int total = 0;

            GrabbableObject[] array = UnityEngine.Object.FindObjectsOfType<GrabbableObject>();
            GrabbableObject[] sortedArray = array.Where(obj => obj.itemProperties.isScrap && !obj.isInShipRoom && !obj.isInElevator).OrderBy(obj => obj.itemProperties.itemName).ToArray();

            if (sortedArray.Length == 0) return "\n\n\nNo objects were found.\n\n";

            foreach (var obj in sortedArray)
            {
                items += $"\n* {obj.itemProperties.itemName} // ${obj.scrapValue}";
                total += obj.scrapValue;
            }

            return $"\n\n\nThere are {sortedArray.Length} objects outside the ship, totalling a value of ${total}.\n{items}\n\n";
        }
    }
}
