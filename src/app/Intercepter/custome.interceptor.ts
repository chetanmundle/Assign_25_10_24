import { HttpInterceptorFn } from '@angular/common/http';

// intercepter need to provide in configfile
export const customeInterceptor: HttpInterceptorFn = (req, next) => {
  console.log('request', req);

  const access_token = localStorage.getItem('access_token');

  const clonedReq = req.clone({
    setHeaders: {
      Authorization: `Bearer ${access_token}`,
    },
  });

  return next(clonedReq);
};
