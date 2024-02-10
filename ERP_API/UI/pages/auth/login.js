import Head from 'next/head'
import Image from 'next/image'
import dynamic from 'next/dynamic'
import Router from 'next/router'
import { useState, CSSProperties } from "react";
import BeatLoader from "react-spinners/BeatLoader";
import Alert from 'react-bootstrap/Alert';
import { omit } from "lodash";


const LoginHeader = dynamic(() => import('../../src/components/layout/loginheader'))

const Login = () => {

    let [loading, setLoading] = useState(false);
    const [errors, setErrors] = useState({});

    const data = {};
    
    const handleSubmit = async (event) => {
        const urlencoded = new URLSearchParams();
        event.preventDefault()
        if(event.target.username.value == '' && event.target.password.value == ''){
            alert("Please fill up all required field");
        }else{   
            urlencoded.append("grant_type", "password");
            urlencoded.append("client_id", "Test");
            urlencoded.append("client_secret", "test123");
            urlencoded.append("scope", "offline_access");
            urlencoded.append("username", event.target.username.value);
            urlencoded.append("password", event.target.password.value);
        }
        
        const endpoint = process.env.NEXT_PUBLIC_BASE_URL+'connect/token'
        const options = {
            method: 'POST',
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded',
                'Accept':'application/json'
            },
            body: urlencoded,
        }
        const response = await fetch(endpoint, options)
        const result = await response.json()
        if(response.status == 200){
            let newObj = omit(errors, "message");
            setErrors(newObj);
            localStorage.setItem("token", result.access_token)
            Router.push('/dashboard')
        }else{
            setErrors({
                ...errors,
                message: result.error_description,
            });
            setLoading(false)
        }
    }
    return (
        <div>
            <LoginHeader />
            <main className="d-flex w-100">
                <div className="container d-flex flex-column">
                    <div className="row vh-100">
                        <div className="col-sm-10 col-md-8 col-lg-6 mx-auto d-table h-100">
                            <div className="d-table-cell align-middle">

                                <div className="text-center mt-4">
                                    <h1 className="h2">Welcome back, Charles</h1>
                                    <p className="lead">
                                        Sign in to your account to continue
                                    </p>
                                </div>

                                <div className="card">
                                    <div className="card-body">
                                        <div className="m-sm-4">
                                            <div className="text-center">
                                                <Image src="/img/avatars/avatar.jpg" alt="Charles Hall" className="img-fluid rounded-circle" width="132" height="132" />
                                            </div>
                                            { errors.message && <Alert key='danger' variant='danger'>{errors.message}</Alert>}
                                            <BeatLoader color="#226fc9" loading={loading} margin={10} size={30} speedMultiplier={1} />
                                            <form onSubmit={handleSubmit}>
                                                <div className="mb-3">
                                                    <label className="form-label">Username</label>
                                                    <input className="form-control form-control-lg" type="text" name="username" placeholder="Enter your Username" required />
                                                </div>
                                                <div className="mb-3">
                                                    <label className="form-label">Password</label>
                                                    <input className="form-control form-control-lg" type="password" name="password" placeholder="Enter your password"  required />
                                                    <small>
                                                        <a href="index.html">Forgot password?</a>
                                                    </small>
                                                </div>
                                                <div className="text-center mt-3">
                                                    <button onClick={() => setLoading(!loading)} className="btn btn-lg btn-primary" type="submit">Submit</button>
                                                </div>
                                            </form>
                                        </div>
                                    </div>
                                </div>

                            </div>
                        </div>
                    </div>
                </div>
            </main>
        </div>
    )
}

export async function getStaticProps(context) {
    return {
      props: {
        protected: false,
      },
    }
}

export default Login

