using System;
using System.Collections.Generic;
using System.Text;

namespace Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private CarDbContext _dataSet;
        public UnitOfWork(CarDbContext dataSet)
        {
            _dataSet = dataSet;
        }
        public void commit()
        {
            _dataSet.SaveChanges();
        }
    }
}
