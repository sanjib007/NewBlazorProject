import Head from "next/head";
import Image from "next/image";
import dynamic from "next/dynamic";
import React, { useState,useEffect } from 'react';
import Button from 'react-bootstrap/Button';
import Modal from 'react-bootstrap/Modal';
import useForm from '../../src/components/formValidator/useForm';
import { omit } from "lodash";

const Add = ({roles,setRole}) => {

    const [show, setShow] = useState(false);
    const handleClose = () => setShow(false);
    const handleShow = () => setShow(true);
    const requiredField = ['roleName']
    const [success, setSuccess] = useState({});
    
    const submitForm = async() => {
          const endpoint = process.env.NEXT_PUBLIC_BASE_URL+'account/roleinsert'
          const JSONdata = {'roleName': values.roleName}
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
          if(result.statusCode == 200){
                setSuccess({...success,msg: "Role Successfully added"});
                setRole([...roles,{'name': values.roleName}])
                console.log(roles)
          }else{
             setSuccess(omit(success, "msg"));
          }
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
                        <label htmlFor="exampleInputUsername">Role Name</label>
                        <input type="text" className="form-control"  onChange={handleChange} name="roleName" id="exampleInputUsername"  placeholder="Enter Role" />
                        { errors.roleName && <small  className="form-text" style={{color: "red"}}>{ errors.roleName }</small> }
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
