using Kaazam.Data;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kaazam.Models
{
    public class Product
    {
        #region Constructor
        public Product()
        {

        }
        #endregion
        #region Properties
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }

        Category Category { get; set; }

        #endregion
    }

    public class Category
    {
        #region Constructor
        public Category()
        {

        }
        #endregion
        #region Properties
        public int Id { get; set; }
        public string Name { get; set; }
        List<Product> Products { get; set; }

        #endregion
    }

    public class CategoryType : ObjectGraphType<Category>
    {
        public CategoryType(IProductRepository productRepository)
        {
            Field(x => x.Id).Description("Category id.");
            Field(x => x.Name, nullable: true).Description("Category name.");

            Field<ListGraphType<ProductType>>(
                "products",
                resolve: context => productRepository.GetProductsWithByCategoryIdAsync(context.Source.Id).Result.ToList()
            );
        }
    }
    public class ProductType : ObjectGraphType<Product>
    {
        public ProductType(ICategoryRepository categoryRepository)
        {
            Field(x => x.Id).Description("Product id.");
            Field(x => x.Name).Description("Product name.");
            Field(x => x.Description, nullable: true).Description("Product description.");
            Field(x => x.Price).Description("Product price.");

            Field<CategoryType>(
                "category",
                resolve: context => categoryRepository.GetCategoryAsync(context.Source.CategoryId).Result
             );
        }
    }

    public class EasyStoreQuery : ObjectGraphType
    {
        public EasyStoreQuery(ICategoryRepository categoryRepository, IProductRepository productRepository)
        {
            Field<CategoryType>(
                "category",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "id", Description = "Category id" }
                ),
                resolve: context => categoryRepository.GetCategoryAsync(context.GetArgument<int>("id")).Result
            );

            Field<ProductType>(
                "product",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "id", Description = "Product id" }
                ),
                resolve: context => productRepository.GetProductAsync(context.GetArgument<int>("id")).Result
            );
        }
    }

    public class EasyStoreSchema : Schema
    {
        public EasyStoreSchema(Func<Type, GraphType> resolveType)
            : base(resolveType)
        {
            Query = (EasyStoreQuery)resolveType(typeof(EasyStoreQuery));
        }
    }


    public class GraphQLQuery
    {
        public string OperationName { get; set; }
        public string NamedQuery { get; set; }
        public string Query { get; set; }
        public string Variables { get; set; }
    }


}
