﻿using MagicT.Shared.Services.Base;

namespace MagicT.Web.Test.Pages.Base;

[Obsolete]
public abstract class SecuredServicePageBase<TModel, TService> : ServicePageBase<TModel,TService>
    where TModel : class, new()
    where TService : IMagicService<TService, TModel>
{
    //public new ISecuredMagicTService<TService, TModel> Service => base.Service as ISecuredMagicTService<TService, TModel>;
}
