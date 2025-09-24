using Resturant.BLL.ModelVM.PaymentVM;

public class PaymentProfile : Profile
{
    public PaymentProfile()
    {
        // ----- Entity → VM -----
        CreateMap<Payment, PaymentVM>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.PaymentID)) // map PK
            .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.OrderID))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.PaymentDate))
            .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => src.PayMethod));

        // ----- VM → Entity -----
        CreateMap<PaymentVM, Payment>()
            .ForMember(dest => dest.PaymentID, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.OrderID, opt => opt.MapFrom(src => src.OrderId))
            .ForMember(dest => dest.PayMethod, opt => opt.MapFrom(src => src.PaymentMethod))
            .ForMember(dest => dest.PaymentDate, opt => opt.MapFrom(src => src.CreatedAt));

        // ----- EditPaymentVM → Entity -----
        CreateMap<EditPaymentVM, Payment>()
            .ForMember(dest => dest.PaymentID, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));

        // ----- CreatePaymentVM → Entity -----
        CreateMap<CreatePaymentVM, Payment>()
            .ForMember(dest => dest.PaymentID, opt => opt.Ignore()) // ignore so DB generates
            .ForMember(dest => dest.OrderID, opt => opt.MapFrom(src => src.OrderId))
            .ForMember(dest => dest.PayMethod, opt => opt.MapFrom(src => src.PaymentMethod))
            .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
            .ForMember(dest => dest.PaymentDate, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(_ => PaymentStatus.Pending))
            .ForMember(dest => dest.IsSuccessful, opt => opt.MapFrom(_ => false))
            .ForMember(dest => dest.TransactionReference, opt => opt.Ignore());
    }
}
