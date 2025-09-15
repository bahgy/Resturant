using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repo.Abstraction
{
    public interface IFeedbackRepo
    {
        IEnumerable<Feedback> GetAll();
        Feedback GetById(int id);
        void Add(Feedback feedback);
        void Update(Feedback feedback);
        void Delete(int id);
    }
    }


