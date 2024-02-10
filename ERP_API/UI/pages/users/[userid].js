import Head from 'next/head'
import Image from 'next/image'
import dynamic from 'next/dynamic'
import React, { useState, useEffect } from 'react';
import { useRouter } from 'next/router'


const LoginHeader = dynamic(() => import('../../src/components/layout/loginheader'))
const MainLayout = dynamic(() => import('../../src/components/layout/mainlayout'))

const UserDetail = (props) =>{
    const { query } = useRouter();

    const [profile , setProfile] = useState();
    const [isLoading, setIsLoading] = useState(true);

    useEffect(() => {

        const fetchData = async () => {
            const endpoint = process.env.NEXT_PUBLIC_BASE_URL+'account/GetProfileByUserId/'+query.userid
            const options = {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': 'Bearer ' + localStorage.getItem("token")
                },
            }
            const response = await fetch(endpoint, options)
            const result = await response.json()
            if(result.statusCode == 200){
                setProfile(result.data)
                setIsLoading(false)
                return result;
            }else{
                return result.statusCode;
            }
        }

        const resultFromApi = fetchData();
    }, []);
    
        if(!isLoading){
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
                                           User Details {profile.userName}
                                        </div>
                                    </div>
                                </div>
                            </div>
        
                        </div>
                    </MainLayout>
                </div>
          )
        }else{
            return (<div></div>)
        }
}

export async function getServerSideProps(context) {
    return {
      props: {
        protected: true,
      }
    }
}
 
export default UserDetail