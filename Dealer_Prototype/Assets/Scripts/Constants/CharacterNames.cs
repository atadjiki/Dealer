using UnityEngine;

namespace Constants
{
    public class Names
    {
        public static readonly string[] MaleFirstNames = new string[]
        {
            "Donovan",
            "Rudy",
            "Bojan",
            "Mike",
            "Royce",
            "Jordan",
            "Joe",
            "Georges",
            "Derrick",
            "Jared",
            "Udoka",
            "Eli",
            "Miye",
            "Juwan"
        };

        public static string RandomMaleFirstName()
        {
            return MaleFirstNames[Random.Range(0, MaleFirstNames.Length)];
        }
    }
}