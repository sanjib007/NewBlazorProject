import Head from 'next/head'
import Image from 'next/image'
import dynamic from 'next/dynamic'
import React, { useState, useEffect } from 'react';
import Router from 'next/router'

const LoginHeader = dynamic(() => import('../../src/components/layout/loginheader'))
const MainLayout = dynamic(() => import('../../src/components/layout/mainlayout'))

function Home() {


    useEffect(() => {
        /** const fetchData = async () => {
            const data = { support_office: 'Banani Office' }
            const endpoint = 'http://127.0.0.1:8000/api/taskmanager/report/supervisor/monthwiseinstallation'
            const JSONdata = JSON.stringify(data)
            const options = {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': 'Bearer ' + localStorage.getItem('token')
                },
               // body: JSONdata,
            }
            const response = await fetch(endpoint, options)
            const result = await response.json()
            return result;
        }

        const resultFromApi = fetchData().catch(console.error);
        console.log(resultFromApi); **/
    });

        return (
            <div>
                <MainLayout>
                    <div className="container-fluid p-0">
    
                        <h1 className="h3 mb-3">Blank Page</h1>
    
                        <div className="row">
                            <div className="col-12">
                                <div className="card">
                                    <div className="card-header">
                                        <h5 className="card-title mb-0">Empty card</h5>
                                    </div>
                                    <div className="card-body">
                                        Welcome to Dashboard
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
    return {
      props: {
        protected: true,
      },
    }
}
  
  export default Home