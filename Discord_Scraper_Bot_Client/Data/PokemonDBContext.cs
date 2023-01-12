using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord_Scraper_Bot_Client.Data
{
    internal class PokemonDBContext : DbContext
    {
        public DbSet<Pokemon> Pokemons { get; set; }

        public PokemonDBContext(DbContextOptions<PokemonDBContext> options) : base(options)
        {

        }

    }
}
