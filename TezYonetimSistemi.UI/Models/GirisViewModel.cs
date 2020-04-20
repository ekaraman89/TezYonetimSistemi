using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TezYonetimSistemi.Model;
using TezYonetimSistemi.Services;

namespace TezYonetimSistemi.UI.Models
{
    public class GirisViewModel
    {
        [Required]
        [EmailAddress]
        public string Mail { get; set; }

        [Required]
        public string Sifre { get; set; }

        public Kullanici GirisKontrol()
        {
            return new KullaniciService().KullanicilariGetir().ToList().Where(x => x.Mail == this.Mail && x.Sifre == this.Sifre).SingleOrDefault();
        }

    }
}