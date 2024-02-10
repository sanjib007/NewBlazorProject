import { useEffect } from "react";
import Router from 'next/router'
import Script from 'next/script'

function MyApp({ Component, pageProps }) {
  useEffect(() => {
    if(pageProps.protected && localStorage.getItem('token') === null) {
      Router.push('/auth/login')
    }else if(pageProps.protected == false && localStorage.getItem('token') === null){
      Router.push('/auth/login')
    }
  }, [pageProps.protected]);
  return <Component {...pageProps} />
}

export default MyApp

