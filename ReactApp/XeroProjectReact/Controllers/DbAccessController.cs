﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

using XeroProjectReact.Models;

namespace XeroProjectReact.Controllers
{
    public class DbAccessController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        private static NpgsqlConnection OpenDB()
        {
            var con = new NpgsqlConnection("Server=deathoftombstone.cfbtbnwur0gl.ap-southeast-2.rds.amazonaws.com;Port=5432;Database=DeathOfTombstone;User Id=Tombstone;Password=xerotomb");
            con.Open();
            return con;
        }

        public List<Character> InitialiseCharacter()
        {
            var connection = OpenDB();
            var command = new NpgsqlCommand("SELECT * FROM charcter", connection);
            var reader = command.ExecuteReader();

            List<Character> characterList = new List<Character>();
            while (reader.Read())
            {
                Character character = new Character();
                character.id = reader.GetInt32(0);
                character.characterName = reader.GetString(1);
                character.characterDescription = reader.GetString(2);
                character.locationId = CheckNullIntegers(reader, 3); 
                characterList.Add(character);
            }

            Console.WriteLine(characterList);
            return characterList;
        }

        public List<Location> InitialiseLocations()
        {
            var connection = OpenDB();
            var command = new NpgsqlCommand("SELECT * FROM location", connection);
            var reader = command.ExecuteReader();

            List<Location> locations = new List<Location>();

            while (reader.Read())
            {

                Location location = new Location();
                location.id = reader.GetInt32(0);
                location.name = reader.GetString(1);
                location.east = CheckNullIntegers(reader, 2);
                location.west = CheckNullIntegers(reader, 3);
                location.north = CheckNullIntegers(reader, 4);
                location.south = CheckNullIntegers(reader, 5);
                location.description = reader.GetString(6);
                location.verboseDescription = reader.GetString(7);
                locations.Add(location);

            }

            Console.WriteLine(locations);

            return locations;
        }

        public int? CheckNullIntegers(NpgsqlDataReader reader, int index)
        {
            if (reader.IsDBNull(index))
                return null;
            else
                return reader.GetInt32(index);
        }


        public List<Item> InitialiseItems()
        {
            var connection = OpenDB();
            var command = new NpgsqlCommand("SELECT * FROM item", connection);
            var reader = command.ExecuteReader();

            List<Item> items = new List<Item>();

            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                Item item = Item.GenerateChildOfItemFromId(id);
                item.id = id;
                item.name = reader.GetString(1);
                item.description = reader.GetString(2);
                item.locationId = CheckNullIntegers(reader, 3);
                item.isVisible = reader.GetBoolean(4);
                // return item casted to its type!
                items.Add(item);
            }
            connection.Close();
            reader.Close();

            Console.WriteLine(items);
            return items;
        }
    }
}