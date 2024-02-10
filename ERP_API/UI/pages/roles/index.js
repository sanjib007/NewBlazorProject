import Head from 'next/head'
import Image from 'next/image'
import dynamic from 'next/dynamic'
import React, { useState, useEffect } from 'react';
import Router from 'next/router'
import MUIDataTable from 'mui-datatables';
import DotLoader from "react-spinners/DotLoader";

const LoginHeader = dynamic(() => import('../../src/components/layout/loginheader'))
const MainLayout = dynamic(() => import('../../src/components/layout/mainlayout'))
const Add = dynamic(() => import('./add'))

function Roles() {

    const [roles , setRole] = useState([]);
    //const [selectedRow, setSelectedRow] = useState(null);
    let [loading, setLoading] = useState(true);

    const columns = [
        { label: 'ID', name: 'id', options: { display: false }},
        { label: 'Role Name', name: 'name' },
        {
            label: "Actions",
            name: "",
            options: {
                customBodyRender: (value, tableMeta, updateValue) => {
                    return (
                        <div>
                            <button className="btn btn-sm btn-danger" onClick={() => deleteRoleFromList(tableMeta) } >Delete</button>
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

    const deleteRoleFromList = (tableMeta) => {
        if (!confirm('Are you sure?')) return false;
        setRole((roles) => roles.filter((i, index) => i.id !== tableMeta.rowData[0]));
        const data = { }
        const endpoint =process.env.NEXT_PUBLIC_BASE_URL+'account/DeleteRole/'+tableMeta.rowData[0]
        const JSONdata = JSON.stringify(data)
        const options = {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + localStorage.getItem('token')
            },
            // body: JSONdata,
        }
        const response = fetch(endpoint, options)
    }

    useEffect(() => {
        const fetchData = async () => {
            const data = { }
            const endpoint = process.env.NEXT_PUBLIC_BASE_URL+'account/GetAllRoles'
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
                setRole(result.data);
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
                                        <h5 className="card-title mb-0">
                                            <Add roles={roles} setRole={setRole} />
                                        </h5>
                                    </div>
                                    <div className="card-body">
                                        <DotLoader color="#226fc9" cssOverride={{ textAlign: 'center' }} loading={loading} margin={10} size={30} speedMultiplier={1} />
                                        <MUIDataTable columns={columns} data={roles} title='Roles' options={options}  />
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
  
  export default Roles