using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoPetV2.Models
{
    public class Usuario
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string NomePet{get; set; }

        public string TipoPet { get; set; }

        public string HistSaude { get; set; }

        public Usuario Clone() => MemberwiseClone() as Usuario;

        public (bool IsValid, string ErrorMessage) Validate()
        {
            if (string.IsNullOrWhiteSpace(NomePet))
            {
                return (false, $"{nameof(NomePet)} is required.");
            }
            else if (string.IsNullOrWhiteSpace(TipoPet))
            {
                return (false, $"{nameof(TipoPet)} is required.");
            }
            else if (string.IsNullOrWhiteSpace(HistSaude))
            {
                return (false, $"{nameof(HistSaude)} is required.");
            }
            else if (string.IsNullOrWhiteSpace(Doencas))
            {
                return (false, $"{nameof(Doencas)} is required.");
            }
            return (true, null);
        }

    }
}
