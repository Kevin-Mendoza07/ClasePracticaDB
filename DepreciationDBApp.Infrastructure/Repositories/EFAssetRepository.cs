using DepreciationDBApp.Domain.Entities;
using DepreciationDBApp.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepreciationDBApp.Infrastructure.Repositories
{
    public class EFAssetRepository : IAssetRepository
    {
        public IDepreciationDbContext depreciationDbContext;

        public EFAssetRepository(IDepreciationDbContext depreciationDbContext)
        {
            this.depreciationDbContext = depreciationDbContext;
        }
        public void Create(Asset t)
        {
            depreciationDbContext.Assets.Add(t);
            depreciationDbContext.SaveChanges();
        }

        public bool Delete(Asset t)
        {
            try
            {
                if (t == null)
                {
                    throw new ArgumentNullException("El objeto Asset no puede ser null");
                }

                Asset asset = FindById(t.Id);
                if (asset == null)
                {
                    throw new ArgumentNullException($"El objeto id:{t.Id} no existe");
                }
                depreciationDbContext.Assets.Remove(asset);
                int result=depreciationDbContext.SaveChanges();

                return result > 0;
            }
            catch
            {
                throw;
            }
        }

        public Asset FindByCode(string code)
        {
            try
            {
                if (String.IsNullOrEmpty(code))
                {
                    throw new ArgumentNullException($"El parametro code {code} no tiene el formato apropiado");
                }
                return depreciationDbContext.Assets.FirstOrDefault(x => x.Code.Equals(code));
            }
            catch
            {
                throw;
            }
        }

        public Asset FindById(int id)
        {
            try
            {
                if (id<0)
                {
                    throw new ArgumentNullException($"El objeto con id {id} no existe");
                }
                return depreciationDbContext.Assets
                                            .FirstOrDefault(x => x.Id == id);
            }
            catch
            {
                throw;
            }
        }

        public List<Asset> FindByName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("La propiedad name no puede ser null");
            }
            return depreciationDbContext.Assets.Where(x => x.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase)).ToList();
        }

        public List<Asset> GetAll()
        {
            return depreciationDbContext.Assets.ToList();
        }

        public int Update(Asset t)
        {
            try
            {
                if (t == null)
                {
                    throw new ArgumentNullException("El objeto Asset no puede ser null");
                }
                Asset asset = FindById(t.Id);
                if (asset == null)
                {
                    throw new Exception($"El objeto con id {t.Id} no existe");
                }

                asset.Name = t.Name;
                asset.Description = t.Description;
                asset.Code = t.Code;
                asset.Id = t.Id;
                asset.IsActive = t.IsActive;
                asset.AmountResidual = t.AmountResidual;
                asset.Amount = t.Amount;
                asset.Status = t.Status;
                asset.Terms = t.Terms;

                depreciationDbContext.Assets.Update(asset);
                return depreciationDbContext.SaveChanges();


            }
            catch
            {
                throw;
            }
        }
    }
}
