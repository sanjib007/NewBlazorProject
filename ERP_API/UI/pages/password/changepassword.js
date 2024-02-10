import Head from "next/head";
import Image from "next/image";
import dynamic from "next/dynamic";
import React, { useState,useEffect } from 'react';
import Button from 'react-bootstrap/Button';
import useForm from '../../src/components/formValidator/useForm';
import { omit } from "lodash";

const LoginHeader = dynamic(() => import('../../src/components/layout/loginheader'))
const MainLayout = dynamic(() => import('../../src/components/layout/mainlayout'))

function ChangePassword() {

    const requiredField = ['password','new_password','confirm_new_password']
    const [success, setSuccess] = useState({});
    
    const submitForm = async() => {
          const endpoint = process.env.NEXT_PUBLIC_BASE_URL+'account/ChangePassword'
          const JSONdata = {'currentPassword': values.password, 'newPassword': values.new_password, 'confirmPassword': values.confirm_new_password}
          const options = {
              method: 'POST',
              headers: {
                  'Content-Type': 'application/json',
                  'Authorization': 'Bearer ' + localStorage.getItem('token')
              },
              body:JSON.stringify(JSONdata)
          }

          if (Object.keys(errors).length === 0) {
                const response = await fetch(endpoint, options)
                const result = await response.json()
                if(result.statusCode == 200){
                    setSuccess({...success,msg: result.message});
                }if(result.StatusCode == 422){
                    setSuccess({...success,msg: result.Message});
                }

               /**
                * else{
                    setSuccess(omit(success, "msg"));
                }
                *  */ 
          }

          
    }

    const {handleChange, values,errors,handleSubmit} = useForm(submitForm,requiredField);
    
    return (
        <div>
            <MainLayout>
                <div className="container-fluid p-0">
                    <div className="row">
                        <div className="col-12">
                            <div className="card">
                                <div className="card-header">
                                    <h5 className="card-title mb-0"> Change Password </h5>
                                    <h1>
                                        { errors.message && <small  className="form-text" style={{color: "red"}}>{ errors.message }</small> }
                                        { success.msg && <small  className="form-text" style={{color: "red"}}>{ success.msg }</small> }
                                    </h1>
                                </div>
                                
                                <div className="card-body">
                                    <form onSubmit={handleSubmit}>
                                        <div className="form-group">
                                            <label htmlFor="exampleInputUsername">Current Password</label>
                                            <input type="text" className="form-control"  onChange={handleChange} name="password" id="password"  placeholder="Enter Current Password" />
                                            { errors.password && <small  className="form-text" style={{color: "red"}}>{ errors.password }</small> }
                                        </div>
                                        <br />
                                        <div className="form-group">
                                            <label htmlFor="exampleInputUsername">New Password</label>
                                            <input type="text" className="form-control"  onChange={handleChange} name="new_password" id="new_password"  placeholder="Enter New Password" />
                                            { errors.new_password && <small  className="form-text" style={{color: "red"}}>{ errors.new_password }</small> }
                                        </div>
                                        <br />
                                        <div className="form-group">
                                            <label htmlFor="exampleInputUsername">Confirm New Password</label>
                                            <input type="text" className="form-control"  onChange={handleChange} name="confirm_new_password" id="confirm_new_password"  placeholder="Enter New Password" />
                                            { errors.confirm_new_password && <small  className="form-text" style={{color: "red"}}>{ errors.confirm_new_password }</small> }
                                        </div>
                                        <br />
                                        <Button type="submit" variant="primary">Change</Button>
                                    </form>
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
  
  export default ChangePassword