using MentalHealthApp.BusinessLogic.Base;
using MentalHealthApp.BusinessLogic.Implementation.Forum.ViewModels;
using MentalHealthApp.BusinessLogic.Implementation.Reviews.ViewModel;
using MentalHealthApp.Entities;
using MentalHealthApp.Entities.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthApp.BusinessLogic.Implementation.Reviews
{
    public class DoctorReviewService : BaseService
    {
        public DoctorReviewService (ServiceDependencies dependencies) : base(dependencies) { }

        public Guid TakeDoctorIdByEmail(string email)
        {
            return ExecuteInTransaction(uow =>
            {

                var id = uow.IdentityUsers.Get()
                                                .Where(cd => cd.Email.Equals(email))
                                                .Select(cd => cd.Id)
                                                .Single();
                
                return id;

            });

        }
        public void  NewReview (string newReview, int stars, string email)
        {

            ExecuteInTransaction(uow =>
            {
                var doctorId = TakeDoctorIdByEmail(email);
                var review = new DoctorReviews();
                review.Id = Guid.NewGuid();
                review.PacientId = (Guid)CurrentUser.Id;
                review.DoctorId = doctorId;
                review.Review = newReview;
                review.CommentDate = DateTime.UtcNow;
                review.StarsNumber = stars;
                uow.DoctorReviews.Insert(review);
                uow.SaveChanges();

            });
        }

        public async Task<DoctorReviews> DeleteReview(Guid id)
        {

            return ExecuteInTransaction(uow =>
            {

                var comment = uow.DoctorReviews.Get()
                                                .Where(cd => cd.Id.Equals(id))
                                                .Single();
                uow.DoctorReviews.Delete(comment);
                uow.SaveChanges();
                return comment;

            });
        }
        public List<DoctorReviewVM> ViewDoctorReviews(Guid doctorId)
        {
            return ExecuteInTransaction(uow =>
            {
                return uow.DoctorReviews.Get()
                                             .Include(cd => cd.Pacient)
                                             .ThenInclude(u => u.User)
                                             .Where(cd => cd.DoctorId.Equals(doctorId))
                                             .Select(cd => new DoctorReviewVM
                                             {
                                                 PacientName = uow.IdentityUsers.Get()
                                                         .Where(u => u.Id.Equals(cd.PacientId))
                                                          .Select(u => $"{u.LastName} {u.FirstName}")
                                                          .Single(),
                                                 Review = cd.Review,
                                                 CommentDate = cd.CommentDate,
                                                 StarsNumber = cd.StarsNumber,
                                                 UserImage = uow.IdentityUsers.Get().Where(u => u.Id.Equals(cd.PacientId)).Select(u => u.UserImage).Single(),
                                             })
                                             .OrderBy(u => u.CommentDate)
                                             .ToList();
            });
        }

        public double GetStarsAverageRating(Guid id)
        {
            var averageRating = UnitOfWork.DoctorReviews.Get().Where(sr => sr.DoctorId.Equals(id)).ToList();
            if(averageRating.Count > 0)
            {
                double averageRatingAverage = averageRating.Average(cd => cd.StarsNumber);
                return averageRatingAverage;
            }
            //
            return 0;
        }
    }
}
