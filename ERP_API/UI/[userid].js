import Head from 'next/head'
import Image from 'next/image'
import dynamic from 'next/dynamic'
import React, { useState, useEffect } from 'react';
import { useRouter } from 'next/router'

const LoginHeader = dynamic(() => import('../../src/components/layout/loginheader'))
const MainLayout = dynamic(() => import('../../src/components/layout/mainlayout'))

const userDetail = (props) =>{

    const [profile , setProfile] = useState({});
    const [uid, setUserid] = useState(props.userid);

    useEffect(() => {  
        
        const fetchData = async () => {
            const endpoint = 'https://dialreport.link3.net/IdentityServer/api/Account/Profile?userId='+uid
            const options = {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': 'Bearer ' + localStorage.getItem('token')
                },
            }
            const response = await fetch(endpoint, options)
            if(response.status == 200){
                const result = await response.json()
                setProfile(result);
            }else{
                return response.status;
            }
        }
        const resultFromApi = fetchData();
        
    }, []);

    
        return (
            <div>
                <MainLayout>
                    <div className="container-fluid p-0">
    
                        <div className="row">
                            <div className="col-12">
                                <div className="card">
                                    <div className="card-header">
                                        <h5 className="card-title mb-0"></h5>
                                    </div>
                                    <div className="card-body">
                                       User Details { profile.username } { profile.email }
                                    </div>
                                </div>
                            </div>
                        </div>
    
                    </div>
                </MainLayout>
            </div>
      )
}

export async function getStaticProps(context) {
    const  { userid } = context.params;
    return {
      props: {
        protected: true,
        userid
      },
    }
}

export async function getStaticPaths() {
    return {
        paths: [],
        fallback: 'blocking' // false or 'blocking'
    };
}
  
  export default userDetail