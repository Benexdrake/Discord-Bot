global using Microsoft.Extensions.Hosting;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;

global using Serilog;

global using System;
global using System.Collections.Generic;
global using System.Linq;
global using System.Reflection;
global using System.Threading.Tasks;

global using Newtonsoft.Json;
global using System.Net.Http.Json;

global using Scraper_Bot;
global using Scraper_Bot.Commands.Slash;
global using Scraper_Bot.Logic;
global using Scraper_Bot.Interfaces;
global using Scraper_Bot.Interfaces.Services;
global using Scraper_Bot.Services;

global using Discord;
global using Discord.Commands;
global using Discord.WebSocket;

global using Webscraper.API;
global using Webscraper.API.Interfaces;
global using Webscraper.API.Scraper.Amazon.Controllers;
global using Webscraper.API.Scraper.Crunchyroll.Controllers;
global using Webscraper.API.Scraper.Dota2.Controllers;
global using Webscraper.API.Scraper.Honda.Controllers;
global using Webscraper.API.Scraper.IMDB.Controllers;
global using Webscraper.API.Scraper.Insight_Digital_Handy.Controllers;
global using Webscraper.API.Scraper.OnePlus.Controllers;
global using Webscraper.API.Scraper.Pokemons.Controller;
global using Webscraper.API.Scraper.Steam.Controllers;
global using Webscraper.API.Scraper.TCG_Magic.Controller;
global using Webscraper.API.Scraper.TCG_Pokemon.Controller;
global using Webscraper.API.Scraper.TVProgramm.Controllers;
global using Webscraper.API.Scraper.Twitch.Controllers;

global using Webscraper.Models.Amazon.Models;
global using Webscraper.Models.Crunchyroll.Models;
global using Webscraper.Models.Crunchyroll.BuildModels;
global using Webscraper.Models.Dota2.Models;
global using Webscraper.Models.Honda.Models;
global using Webscraper.Models.Insight_Digital_Handy.Models;
global using Webscraper.Models.IMDB.BuildModels;
global using Webscraper.Models.IMDB.Models;
global using Webscraper.Models.Pokemons.Builder;
global using Webscraper.Models.Pokemons.Models;
global using Webscraper.Models.Steam.Models;
global using Webscraper.Models.TCG_Magic.Model;
global using Webscraper.Models.TCG_Pokemon.Models;
global using Webscraper.Models.TVProgramm.Models;
global using Webscraper.Models.Twitch.Models;
