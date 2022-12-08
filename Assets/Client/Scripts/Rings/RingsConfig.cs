using System.Collections;
using UnityEngine;

namespace Client.Rings
{
    public static class RingsConfig
    {
        private static RingsSetuper _RingsSetuper;

        public static RingsSetuper RingsSetuper { get => _RingsSetuper; set => _RingsSetuper = value; }
    }
}