using AutoMapper;
using Ecommerce.Application.Features.Orders.Vms;
using Ecommerce.Application.Persistence;
using Ecommerce.Domain;
using MediatR;

namespace Ecommerce.Application.Features.Orders.Commands.UpdateOrder;

public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, OrderVm>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public UpdateOrderCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<OrderVm> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _unitOfWork.Repository<Order>().GetByIdAsync(request.OrderId);
        order.Status = request.Status;

        _unitOfWork.Repository<Order>().UpdateEntity(order);
        var result = await _unitOfWork.Complete();

        if (result <= 0)
        {
            throw new Exception("Error trying to update the ordership status");
        }
        return _mapper.Map<OrderVm>(order);
    }
}