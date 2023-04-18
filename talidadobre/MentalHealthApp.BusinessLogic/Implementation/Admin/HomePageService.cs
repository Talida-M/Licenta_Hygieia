using MentalHealthApp.BusinessLogic.Base;
using MentalHealthApp.BusinessLogic.Implementation.Admin.Validations;
using MentalHealthApp.BusinessLogic.Implementation.Admin.ViewModels;
using MentalHealthApp.Common.Exceptions;
using MentalHealthApp.Common.Extensions;
using MentalHealthApp.Entities.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthApp.BusinessLogic.Implementation.Admin
{
    public class HomePageService : BaseService
    {
        private readonly ArticleValidator _articleValidator;
        public HomePageService(ServiceDependencies dependencies) : base(dependencies)
        {
            _articleValidator = new ArticleValidator();
        }

        public byte[] ConvertToBytes(IFormFile image)
        {
            BinaryReader reader = new BinaryReader(image.OpenReadStream());
            var imageBytes = reader.ReadBytes((int)image.Length);
            return imageBytes;
        }
        public void AddCondition(ConditionsPostVM model)
        {
            ExecuteInTransaction(uow =>
            {
                var simptom = uow.ConditionsPosts.Get()
                                            .SingleOrDefault(s => s.ConditionName.Equals(model.ConditionName));
                if (simptom != null)
                {
                    string message = "Conditia " + model.ConditionName + " este deja inregistrat in baza de date";
                    throw new AlreadyExistsInDB(nameof(ConditionsPost), message);
                }
                else
                {
                    var condition = new ConditionsPost();
                    condition.Id = Guid.NewGuid();
                    condition.ConditionName = model.ConditionName;
                    condition.ShortDecsription = model.ShortDecsription;
                    condition.Description = model.Description;
                    condition.Image = ConvertToBytes(model.ImageForm);
                    uow.ConditionsPosts.Insert(condition);
                    uow.SaveChanges();
                }

            });


        }

        public async Task<ConditionsPostVM> GetConditionByTitle(string name)
        {
            return ExecuteInTransaction(uow =>
            {
                var condition = uow.ConditionsPosts.Get().Select(s => new ConditionsPostVM
                {
                    Id = s.Id,
                    ConditionName = s.ConditionName,
                    ShortDecsription = s.ShortDecsription,
                    Description = s.Description,
                    Image = s.Image
                }).FirstOrDefault(s => s.ConditionName.Equals(name));

                return condition;

            });

        }

        public ConditionsPostVM GetCondition(Guid id)
        {
            return ExecuteInTransaction( uow =>
            {
                var condition =  uow.ConditionsPosts.Get().Select(s => new ConditionsPostVM
                {
                    Id = s.Id,
                    ConditionName = s.ConditionName,
                    ShortDecsription = s.ShortDecsription,
                    Description = s.Description,
                    Image = s.Image
                }).First(s => s.Id.Equals(id));

                    return condition;


            });

        }
        public List<ConditionsPostVM> GetAllConditions()
        {
            return ExecuteInTransaction(uow =>
            {
                var condition = uow.ConditionsPosts.Get().Select(s => new ConditionsPostVM
                {
                    Id = s.Id,
                    ConditionName = s.ConditionName,
                    ShortDecsription = s.ShortDecsription,
                    Description = s.Description,
                    Image = s.Image
                }).ToList();

                return condition;

            });
        }

        public List<ConditionsPostVM> GetPageConditions(int pages, int numberOnPage)
        {
            return ExecuteInTransaction(uow =>
            {
                var condition = uow.ConditionsPosts.Get()
                                                    .Skip(pages).Take(numberOnPage)
                                                    .Select(s => new ConditionsPostVM
                                                    {
                                                        Id = s.Id,
                                                        ConditionName = s.ConditionName,
                                                        ShortDecsription = s.ShortDecsription,
                                                        Description = s.Description,
                                                        Image = s.Image
                                                    }).ToList();

                return condition;

            });
        }
        public List<ConditionsPostVM> GetFourConditions()
        {
            return ExecuteInTransaction(uow =>
            {
                var condition = uow.ConditionsPosts.Get().Select(s => new ConditionsPostVM
                {
                    Id = s.Id,
                    ConditionName = s.ConditionName,
                    ShortDecsription = s.ShortDecsription,
                    Description = s.Description,
                    Image = s.Image
                }).Take(4).ToList();

                return condition;

            });
        }
        //TopReads


        public void AddArticle(TopReadsVM model)
        {
            _articleValidator.Validate(model).ThenThrow(model);
            ExecuteInTransaction(uow =>
            {
                var simptom = uow.TopReads.Get()
                                            .SingleOrDefault(s => s.Id.Equals(model.Id));
                if (simptom != null)
                {
                    string message = "Articolul " + model.ArticleTile + " este deja inregistrat in baza de date";
                    throw new AlreadyExistsInDB(nameof(TopReads), message);
                }
                else
                {
                    var article = new TopReads();
                    article.Id = Guid.NewGuid();
                    article.ArticleTile = model.ArticleTile;
                    article.Author = model.Author;
                    article.Content = model.Content;
                    article.ArticleImage = ConvertToBytes(model.ImageForm);
                    uow.TopReads.Insert(article);
                    uow.SaveChanges();
                }

            });


        }

        public async Task<TopReadsVM> GetArticleById(Guid id)
        {
            return ExecuteInTransaction(uow =>
            {
                var condition = uow.TopReads.Get().Select(s => new TopReadsVM
                {
                    Id = s.Id,
                    ArticleTile = s.ArticleTile,
                    Author = s.Author,
                    Content = s.Content,
                    ArticleImage = s.ArticleImage
                }).FirstOrDefault(s => s.Id.Equals(id));

                return condition;

            });

        }
        public List<TopReadsVM> GetAllArticles()
        {
            return ExecuteInTransaction(uow =>
            {
                var articles = uow.TopReads.Get()
                                  .Select(s => new TopReadsVM
                                    {
                                        Id = s.Id,
                                        ArticleTile = s.ArticleTile,
                                        Author = s.Author,
                                        Content = s.Content,
                                        ArticleImage = s.ArticleImage
                                    }).ToList();

                return articles;

            });
        }

        public List<TopReadsVM> GetPageArticles(int pages, int numberOnPage)
        {
            return ExecuteInTransaction(uow =>
            {
                var articles = uow.TopReads.Get()
                                    .Skip(pages).Take(numberOnPage)
                                  .Select(s => new TopReadsVM
                                  {
                                      Id = s.Id,
                                      ArticleTile = s.ArticleTile,
                                      Author = s.Author,
                                      Content = s.Content,
                                      ArticleImage = s.ArticleImage
                                  }).ToList();

                return articles;

            });
        }
        public List<TopReadsVM> GetFourArticles()
        {
            return ExecuteInTransaction(uow =>
            {
                var articles = uow.TopReads.Get()
                                  .Select(s => new TopReadsVM
                                  {
                                      Id = s.Id,
                                      ArticleTile = s.ArticleTile,
                                      Author = s.Author,
                                      Content = s.Content,
                                      ArticleImage = s.ArticleImage
                                  }).Take(4).ToList();
               

                return articles;

            });
        }
    }
}
