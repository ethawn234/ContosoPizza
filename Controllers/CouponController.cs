using ContosoPizza.Services;
using ContosoPizza.Models;
using Microsoft.AspNetCore.Mvc;
using ContosoPizza.Data;
using Microsoft.EntityFrameworkCore;

namespace ContosoPizza.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CouponController(PromotionsContext context, ILogger<CouponController> logger) : ControllerBase // A PromotionsContext is injected into controller via Primary Constructor DI.
{
  
  PromotionsContext _context = context;
  private readonly ILogger _logger = logger;

  /// <summary>
    /// View the Promotions
    /// </summary>
    /// <returns>List of Promotions</returns>
  [HttpGet]
  [Produces("application/json")]
  public IEnumerable<Coupon> Get()
  {
    _logger.LogInformation("Fetching coupons...");

    foreach(var coupon in _context.Coupons)
    {
      _logger.LogInformation("INFO: Id: {}, Expiration: {}, Description: {}", coupon.Id, coupon.Expiration, coupon.Description);
      _logger.LogCritical("CRITICAL: Id: {}, Expiration: {}, Description: {}", coupon.Id, coupon.Expiration, coupon.Description);
      _logger.LogWarning("WARN: Id: {}, Expiration: {}, Description: {}", coupon.Id, coupon.Expiration, coupon.Description);
    }
    return [.. _context.Coupons.AsNoTracking()]; // collection expression; analogous to spread operator syntax.
    // return _context.Coupons.AsNoTracking().ToList();
  }


}


/*
  public class CouponController : ControllerBase
  {
  PromotionsContext _context; 

  public CouponController(PromotionsContext context)
  {
    _context = context; // add the service via constructor DI
  }

  // Get All coupons
  [HttpGet]
  public IEnumerable<Coupon> Get()
  {
    // return [.. _context.Coupons.AsNoTracking()]; // collection expression; analogous to spread operator syntax.
    return _context.Coupons.AsNoTracking().ToList();

  }
  */