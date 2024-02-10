import Head from 'next/head'
import Image from 'next/image'
import dynamic from 'next/dynamic'
import React, { useState, useEffect } from 'react';
import Router from 'next/router'
import MUIDataTable from 'mui-datatables';
import userDetail from './[userid]';
import DotLoader from "react-spinners/DotLoader";

const LoginHeader = dynamic(() => import('../../src/components/layout/loginheader'))
const MainLayout = dynamic(() => import('../../src/components/layout/mainlayout'))
const Add = dynamic(() => import('./add'))

function Users() {

    const [data , setData] = useState();
    const [selectedRow, setSelectedRow] = useState(null);
    let [loading, setLoading] = useState(true);

    const columns = [
        { label: 'Email', name: 'email' },
        { label: 'ID', name: 'id', options: { display: false } },
        { label: 'Phone Number', name: 'phoneNumber' },
        {
            label: 'Phone Number Status',
            name: "phoneNumberConfirmed",
            options: {
                customBodyRender: (value, tableMeta, updateValue) => {
                    return  (value === true) ? <span className="badge bg-success me-1 my-1">Active</span> :<span className="badge bg-danger me-1 my-1">Inactive</span>;
                }
            },
        },
        { label: 'Username', name: 'userName' },
        {
            label: "Actions",
            name: "",
            options: {
                customBodyRender: (value, tableMeta, updateValue) => {
                    return (
                        <div>
                            <button className="btn btn-sm btn-warning" onClick={() => console.log(value, tableMeta) } >Edit</button>
                            &nbsp;
                            <button className="btn btn-sm btn-danger" onClick={() => console.log(value, tableMeta) } >Delete</button>
                            &nbsp;
                            <button className="btn btn-sm btn-success" onClick={() => 
                            Router.push({ pathname: `/users/[userid]`, query: { userid: tableMeta.rowData[1] } })}
                            >View</button>
                        </div>
                    )
                }
            }
        },
    ];

    const options = {
        filterType: 'checkbox',
        filter: true
    };

    useEffect(() => {
        const fetchData = async () => {
            const data = { }
            const endpoint = process.env.NEXT_PUBLIC_BASE_URL+'account/GetAllUsers'
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
            
            if(response.status == 200){
                const result = await response.json()
                setData(result.data);
                setLoading(false)
                return result;
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
                                        <h5 className="card-title mb-0"> <Add data={data} setData={setData} /></h5>
                                    </div>
                                    <div className="card-body">
                                        <DotLoader color="#226fc9" cssOverride={{ textAlign: 'center' }} loading={loading} margin={10} size={30} speedMultiplier={1} />
                                        <MUIDataTable columns={columns} data={data} title='Users' options={options}  />
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
  
  export default Users