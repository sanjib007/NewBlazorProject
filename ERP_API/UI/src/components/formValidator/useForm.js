import React, { useState } from "react";
import { omit } from "lodash";

const useForm = (callback , requiredField) => {

  const [values, setValues] = useState({});
  const [errors, setErrors] = useState({});

  const validate = (event, name, value) => {
    switch (name) {
        case "username":
            if (value.length <= 1 || value == '') {
            setErrors({
                ...errors,
                username: "Username atleast have 2 letters",
            });
            } else {
            let newObj = omit(errors, "username");
            setErrors(newObj);
            }
            break;
        case "roleName":
            if (value.length <= 1 || value == '') {
            setErrors({
                ...errors,
                roleName: "Role atleast have 2 letters",
            });
            } else {
            let newObj = omit(errors, "roleName");
            setErrors(newObj);
            }
            break;
        case "email":
            if (
            !new RegExp(
                /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/
            ).test(value) || value == '' ) {
            setErrors({
                ...errors,
                email: "Enter a valid email address",
            });
            } else {
            let newObj = omit(errors, "email");
            setErrors(newObj);
            }
            break;

        case "password":
            if (
            !new RegExp(/^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{8,}$/).test(value)
            ) {
            setErrors({
                ...errors,
                password:
                "Password should contains atleast 8 charaters and containing uppercase,lowercase and numbers",
            });
            } else {
            let newObj = omit(errors, "password");
            setErrors(newObj);
            }
            break;

        case "new_password":
                if (
                !new RegExp(/^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{8,}$/).test(value)
                ) {
                setErrors({
                    ...errors,
                    new_password:
                    "Password should contains atleast 8 charaters and containing uppercase,lowercase and numbers",
                });
                } else {
                let newObj = omit(errors, "new_password");
                setErrors(newObj);
                }
                break;
        case "confirm_new_password":
                if (
                !new RegExp(/^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{8,}$/).test(value)
                ) {
                setErrors({
                    ...errors,
                    confirm_new_password:
                    "Password should contains atleast 8 charaters and containing uppercase,lowercase and numbers",
                });
                }else if(values.new_password == "" || values.new_password!=value){
                    setErrors({
                        ...errors,
                        confirm_new_password:
                        "New Password & Confirm password doesn't match",
                    });
                } else {
                    let newObj = omit(errors, "confirm_new_password");
                    setErrors(newObj);
                }
                break;
        case "role":
            if (value.length == 0 || value == '') {
            setErrors({
                ...errors,
                role: "role can not be blank",
            });
            } else {
                let newObj = omit(errors, "role");
                setErrors(newObj);
            }
        break;

        case "phoneNumber":
            if (value.length < 11 || value.length > 11 || value == '') {
            setErrors({
                ...errors,
                phoneNumber: "PLease add 11 digit phone number",
            });
            } else {
            let newObj = omit(errors, "phoneNumber");
            setErrors(newObj);
            }
            break;
        default:
            break;
    }
  };

  const handleChange = (event) => {
        event.persist();
        let name = event.target.name;
        let val = event.target.value;
        validate(event, name, val);
        setValues({
        ...values,
        [name]: val,
        });
  };

  const handleSubmit = (event) => {
    const erCheck = 0;
    if (event) event.preventDefault();
    if (Object.keys(errors).length === 0 || Object.keys(errors).length !== 0) {
        requiredField.forEach(function (item, index) {
            if(typeof values[item] === "undefined" || values[item] === ""){
                erCheck = 1;
                setErrors({
                    ...errors,
                    message: "Please fill all required field",
                })
            }else{
                let newObj = omit(errors, "message");
                setErrors(newObj);
            }
        }); 

        if (erCheck == 0) {
            callback();
        }
    }
    
  };

  return {values,errors,handleChange,handleSubmit};
};

export default useForm;
