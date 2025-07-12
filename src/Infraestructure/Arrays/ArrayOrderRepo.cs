using Domain.Entities;
using Domain.Repositories;

namespace Infraestructure.Arrays;

public class ArrayOrderRepo : BaseArrayRepository<Order>, IOrderRepo
{
}
