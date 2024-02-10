import Head from "next/head";
import Image from "next/image";
import dynamic from "next/dynamic";
import React, { useState,useEffect } from 'react';
import Button from 'react-bootstrap/Button';
import Modal from 'react-bootstrap/Modal';
import useForm from '../../src/components/formValidator/useForm';
import { omit } from "lodash";

const Add = ({data,setData}) => {

    const [show, setShow] = useState(false);
    const [roles, setRoles] = useState({});
    const handleClose = () => setShow(false);
    const handleShow = () => setShow(true);
    const requiredField = ['username','email','password','role','phoneNumber']
    const [success, setSuccess] = useState({});
    
    const submitForm = async() => {
      
          const endpoint = process.env.NEXT_PUBLIC_BASE_URL+'Administrator/CreateUserByAdmin'
          const JSONdata = {
              'userName':values.username,
              'email':values.email,
              'password':values.password,
              'roles':[values.role],
              'phoneNumber':values.phoneNumber
          }

          const options = {
              method: 'POST',
              headers: {
                  'Content-Type': 'application/json',
                  'Authorization': 'Bearer ' + localStorage.getItem('token')
              },
              body:JSON.stringify(JSONdata)
          }
          const response = await fetch(endpoint, options)
          const result = await response.json()
          if(response.statusCode == 200){
              if(result.succeeded){
                setSuccess({...success,msg: "User Successfully added"});
                setData([...data,JSONdata])
              }
          }else{
             setSuccess(omit(success, "msg"));
          }
    }

    useEffect(() => {
      const fetchData = async () => {
          const endpoint = process.env.NEXT_PUBLIC_BASE_URL+'account/GetAllRoles'
          const options = {
              method: 'GET',
              headers: {
                  'Content-Type': 'application/json',
                  'Authorization': 'Bearer ' + localStorage.getItem('token')
              },
          }
          const response = await fetch(endpoint, options)
          const result = await response.json()
          if(result.statusCode == 200){
              setRoles(result.data);
              return result;
          }else{
              return result.statusCode;
          }
      }

      const resultFromApi = fetchData();
  }, []);

  const rolesDropDown = () => {
    let items = []; 
    if(roles.length > 0){
      items.push(<option key={roles.length+1} value="">SELECT</option>);
      for (let i = 0; i < roles.length; i++) { 
        items.push(<option key={i} value={roles[i]['name']}>{roles[i]['name']}</option>);   
      }
    }      
    return items;
  }

  const {handleChange, values,errors,handleSubmit} = useForm(submitForm,requiredField);
    return (
        <>
          <Button variant="primary" onClick={handleShow}>
            Add User
          </Button>
          <Modal show={show} size="lg" onHide={handleClose} animation={false}>
            <Modal.Header closeButton>
              <Modal.Title>
                    { errors.message && <small  className="form-text" style={{color: "red"}}>{ errors.message }</small> }
                    { success.msg && <small  className="form-text" style={{color: "green"}}>{ success.msg }</small> }
              </Modal.Title>
            </Modal.Header>
            <Modal.Body>
                <form onSubmit={handleSubmit}>
                    <div className="form-group">
                        <label htmlFor="exampleInputUsername">Username</label>
                        <input type="text" className="form-control"  onChange={handleChange} name="username" id="exampleInputUsername"  placeholder="Enter Username" />
                        { errors.username && <small  className="form-text" style={{color: "red"}}>{ errors.username }</small> }
                    </div>

                    <div className="form-group">
                        <label htmlFor="exampleInputEmail1">Email address</label>
                        <input type="email" className="form-control"  onChange={handleChange} id="exampleInputEmail1" name="email"  placeholder="Enter email" />
                        { errors.email && <small  className="form-text" style={{color: "red"}}>{ errors.email }</small> }
                    </div>

                    <div className="form-group">
                        <label htmlFor="exampleInputPassword1">Password</label>
                        <input type="password" className="form-control" onChange={handleChange} id="exampleInputPassword1" name="password" placeholder="Password" />
                        { errors.password && <small  className="form-text" style={{color: "red"}}>{ errors.password }</small> }
                    </div>

                    <div className="form-group">
                        <label htmlFor="exampleInputRole">Role</label>
                        <select className="form-control" onChange={handleChange} name="role">
                            { rolesDropDown() }
                        </select>
                        { errors.role && <small  className="form-text" style={{color: "red"}}>{ errors.role }</small> }
                    </div>

                    <div className="form-group">
                        <label htmlFor="exampleInputPhone">Phone Number</label>
                        <input type="text" className="form-control" onChange={handleChange} id="exampleInputPhone" name="phoneNumber" placeholder="Enter Phone Number" />
                        { errors.phoneNumber && <small  className="form-text" style={{color: "red"}}>{ errors.phoneNumber }</small> }
                    </div>
                    <br />
                    <Button type="submit" variant="primary">Save Changes</Button>
                </form>
            </Modal.Body>
            <Modal.Footer>
              <Button variant="secondary" onClick={handleClose}>
                Close
              </Button>
            </Modal.Footer>
          </Modal>
        </>
      );
}

export default Add
