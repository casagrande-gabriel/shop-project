using Domain.Entities;
using Domain.Repositories;

namespace Infraestructure.List;

public class ListOrderRepo :BaseListRepository<Order>, IOrderRepo
{
}
