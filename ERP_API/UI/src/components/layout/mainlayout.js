import Image from 'next/image'
import Head from 'next/head'
import Router from 'next/router'
import Script from 'next/script'
import React, { useEffect } from 'react';
import Link from 'next/link'

const MainLayout = (props) => {
    
    const logout = async() => {
       /** const endpoint = 'https://dialreport.link3.net/IdentityServer/api/Account/Logout'
        const options = {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': 'Bearer '+localStorage.getItem('token')
            }
        }
        const response = await fetch(endpoint, options)
        if(response.status == 200){
            localStorage.removeItem("token")
            Router.push('/auth/login')
        }
         */

        localStorage.removeItem("token")
        Router.push('/auth/login')
    }

    return (
        <div>
            <Head>
                <meta charSet="utf-8" />
                <meta httpEquiv="X-UA-Compatible" content="IE=edge" />
                <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no" />
                <meta name="description" content="" />
                <meta name="author" content="" />
                <meta name="keywords" content="" />
                <link rel="preconnect" href="https://fonts.gstatic.com" />
                <link rel="shortcut icon" href="img/icons/icon-48x48.png" />
                <link rel="canonical" href="https://demo-basic.adminkit.io/pages-sign-in.html" />
                <link rel="stylesheet"
                    href="https://cdn.jsdelivr.net/npm/bootstrap@5.2.0-beta1/dist/css/bootstrap.min.css"
                    integrity="sha384-0evHe/X+R7YkIZDRvuzKMRqM+OrBnVFBL6DOitfPri4tjfHxaWutUpFmBp4vmVor"
                    crossOrigin="anonymous"
                    />
                <link
                rel="stylesheet"
                href="https://fonts.googleapis.com/icon?family=Material+Icons"
                />
                <title>Sign In | AdminKit Demo </title>
            </Head>
            
            <div className="wrapper">
                <nav id="sidebar" className="sidebar js-sidebar">
                    <div className="sidebar-content js-simplebar">
                        <a className="sidebar-brand" href="#"><span className="align-middle">AdminKit</span></a>
                        <ul className="sidebar-nav">
                            <li className="sidebar-item">
                                <Link href="/dashboard">
                                    <a className="sidebar-link">
                                        <i className="align-middle" data-feather="sliders"></i> <span className="align-middle">Dashboard</span>
                                    </a>
                                </Link>
                            </li>
                            <li className="sidebar-item">
                                <Link href="/roles" >
                                    <a className="sidebar-link">
                                        <i className="align-middle" data-feather="sliders"></i> <span className="align-middle">Roles</span>
                                    </a>
                                </Link>
                            </li>
                            <li className="sidebar-item">
                                <Link href="/users">
                                    <a className="sidebar-link">
                                        <i className="align-middle" data-feather="sliders"></i> <span className="align-middle">Users</span>
                                    </a>
                                </Link>
                            </li>
                            <li className="sidebar-item">
                                <Link href="/password/changepassword">
                                    <a className="sidebar-link">
                                        <i className="align-middle" data-feather="sliders"></i> <span className="align-middle">Change Password</span>
                                    </a>
                                </Link>
                            </li>
                            <li className="sidebar-item">
                                <a className="sidebar-link"  onClick={logout} >
                                    <i className="align-middle" data-feather="user"></i> <span className="align-middle">Logout</span>
                                </a>
                            </li>
                        </ul>
                    </div>
                </nav>

                <div className="main">
                    <nav className="navbar navbar-expand navbar-light navbar-bg">
                        <a className="sidebar-toggle js-sidebar-toggle">
                            <i className="hamburger align-self-center"></i>
                        </a>
                        <div className="navbar-collapse collapse">
                            <ul className="navbar-nav navbar-align">
                                <li className="nav-item dropdown">
                                    <a className="nav-icon dropdown-toggle" href="#" id="alertsDropdown" data-bs-toggle="dropdown">
                                        <div className="position-relative">
                                            <i className="align-middle" data-feather="bell"></i>
                                            <span className="indicator">4</span>
                                        </div>
                                    </a>
                                    <div className="dropdown-menu dropdown-menu-lg dropdown-menu-end py-0" aria-labelledby="alertsDropdown">
                                        <div className="dropdown-menu-header">
                                            4 New Notifications
                                        </div>
                                        <div className="list-group">
                                            <a href="#" className="list-group-item">
                                                <div className="row g-0 align-items-center">
                                                    <div className="col-2">
                                                        <i className="text-danger" data-feather="alert-circle"></i>
                                                    </div>
                                                    <div className="col-10">
                                                        <div className="text-dark">Update completed</div>
                                                        <div className="text-muted small mt-1">Restart server 12 to complete the update.</div>
                                                        <div className="text-muted small mt-1">30m ago</div>
                                                    </div>
                                                </div>
                                            </a>
                                            <a href="#" className="list-group-item">
                                                <div className="row g-0 align-items-center">
                                                    <div className="col-2">
                                                        <i className="text-warning" data-feather="bell"></i>
                                                    </div>
                                                    <div className="col-10">
                                                        <div className="text-dark">Lorem ipsum</div>
                                                        <div className="text-muted small mt-1">Aliquam ex eros, imperdiet vulputate hendrerit et.</div>
                                                        <div className="text-muted small mt-1">2h ago</div>
                                                    </div>
                                                </div>
                                            </a>
                                            <a href="#" className="list-group-item">
                                                <div className="row g-0 align-items-center">
                                                    <div className="col-2">
                                                        <i className="text-primary" data-feather="home"></i>
                                                    </div>
                                                    <div className="col-10">
                                                        <div className="text-dark">Login from 192.186.1.8</div>
                                                        <div className="text-muted small mt-1">5h ago</div>
                                                    </div>
                                                </div>
                                            </a>
                                            <a href="#" className="list-group-item">
                                                <div className="row g-0 align-items-center">
                                                    <div className="col-2">
                                                        <i className="text-success" data-feather="user-plus"></i>
                                                    </div>
                                                    <div className="col-10">
                                                        <div className="text-dark">New connection</div>
                                                        <div className="text-muted small mt-1">Christina accepted your request.</div>
                                                        <div className="text-muted small mt-1">14h ago</div>
                                                    </div>
                                                </div>
                                            </a>
                                        </div>
                                        <div className="dropdown-menu-footer">
                                            <a href="#" className="text-muted">Show all notifications</a>
                                        </div>
                                    </div>
                                </li>
                                <li className="nav-item dropdown">
                                    <a className="nav-icon dropdown-toggle" href="#" id="messagesDropdown" data-bs-toggle="dropdown">
                                        <div className="position-relative">
                                            <i className="align-middle" data-feather="message-square"></i>
                                        </div>
                                    </a>
                                    <div className="dropdown-menu dropdown-menu-lg dropdown-menu-end py-0" aria-labelledby="messagesDropdown">
                                        <div className="dropdown-menu-header">
                                            <div className="position-relative">
                                                4 New Messages
                                            </div>
                                        </div>
                                        <div className="list-group">
                                            <a href="#" className="list-group-item">
                                                <div className="row g-0 align-items-center">
                                                    <div className="col-2">
                                                        <Image src="/img/avatars/avatar-5.jpg" layout='fill' className="avatar img-fluid rounded-circle" alt="Vanessa Tucker" />
                                                    </div>
                                                    <div className="col-10 ps-2">
                                                        <div className="text-dark">Vanessa Tucker</div>
                                                        <div className="text-muted small mt-1">Nam pretium turpis et arcu. Duis arcu tortor.</div>
                                                        <div className="text-muted small mt-1">15m ago</div>
                                                    </div>
                                                </div>
                                            </a>
                                            <a href="#" className="list-group-item">
                                                <div className="row g-0 align-items-center">
                                                    <div className="col-2">
                                                        <Image src="/img/avatars/avatar-2.jpg" layout='fill' className="avatar img-fluid rounded-circle" alt="William Harris" />
                                                    </div>
                                                    <div className="col-10 ps-2">
                                                        <div className="text-dark">William Harris</div>
                                                        <div className="text-muted small mt-1">Curabitur ligula sapien euismod vitae.</div>
                                                        <div className="text-muted small mt-1">2h ago</div>
                                                    </div>
                                                </div>
                                            </a>
                                            <a href="#" className="list-group-item">
                                                <div className="row g-0 align-items-center">
                                                    <div className="col-2">
                                                        <Image src="/img/avatars/avatar-4.jpg" layout='fill' className="avatar img-fluid rounded-circle" alt="Christina Mason" />
                                                    </div>
                                                    <div className="col-10 ps-2">
                                                        <div className="text-dark">Christina Mason</div>
                                                        <div className="text-muted small mt-1">Pellentesque auctor neque nec urna.</div>
                                                        <div className="text-muted small mt-1">4h ago</div>
                                                    </div>
                                                </div>
                                            </a>
                                            <a href="#" className="list-group-item">
                                                <div className="row g-0 align-items-center">
                                                    <div className="col-2">
                                                        <Image src="/img/avatars/avatar-3.jpg" layout='fill' className="avatar img-fluid rounded-circle" alt="Sharon Lessman" />
                                                    </div>
                                                    <div className="col-10 ps-2">
                                                        <div className="text-dark">Sharon Lessman</div>
                                                        <div className="text-muted small mt-1">Aenean tellus metus, bibendum sed, posuere ac, mattis non.</div>
                                                        <div className="text-muted small mt-1">5h ago</div>
                                                    </div>
                                                </div>
                                            </a>
                                        </div>
                                    </div>
                                </li>
                                <li className="nav-item dropdown">
                                    <a className="nav-icon dropdown-toggle d-inline-block d-sm-none" href="#" data-bs-toggle="dropdown">
                                        <i className="align-middle" data-feather="settings"></i>
                                    </a>
                                    <a className="nav-link dropdown-toggle d-none d-sm-inline-block" href="#" data-bs-toggle="dropdown">
                                        <Image src="/img/avatars/avatar.jpg" layout='fill' className="avatar img-fluid rounded me-1" alt="Charles Hall" />
                                        <span className="text-dark">ddd</span>
                                    </a>
                                    <div className="dropdown-menu dropdown-menu-end">
                                        <a className="dropdown-item" href="pages-profile.html"><i className="align-middle me-1" data-feather="user"></i> Profile</a>
                                        <a className="dropdown-item" href="#"><i className="align-middle me-1" data-feather="pie-chart"></i> Analytics</a>
                                        <div className="dropdown-divider"></div>
                                        <a className="dropdown-item" href="index.html"><i className="align-middle me-1" data-feather="settings"></i> Settings & Privacy</a>
                                        <a className="dropdown-item" href="#"><i className="align-middle me-1" data-feather="help-circle"></i> Help Center</a>
                                        <div className="dropdown-divider"></div>
                                        <a className="dropdown-item" href="#">Log out</a>
                                    </div>
                                </li>
                            </ul>
                        </div>
                    </nav>

                    <main className="content">
                        {props.children}
                    </main>
                    
                    <footer className="footer">
                        <div className="container-fluid">
                            <div className="row text-muted">
                                <div className="col-6 text-start">
                                    <p className="mb-0">
                                        <a className="text-muted" href="https://adminkit.io/"  rel="noreferrer" target="_blank"><strong>AdminKit</strong></a> &copy;
                                    </p>
                                </div>
                                <div className="col-6 text-end">
                                    <ul className="list-inline">
                                        <li className="list-inline-item">
                                            <a className="text-muted" href="https://adminkit.io/"  rel="noreferrer" target="_blank">Support</a>
                                        </li>
                                        <li className="list-inline-item">
                                            <a className="text-muted" href="https://adminkit.io/"  rel="noreferrer"  target="_blank">Help Center</a>
                                        </li>
                                        <li className="list-inline-item">
                                            <a className="text-muted" href="https://adminkit.io/"  rel="noreferrer" target="_blank">Privacy</a>
                                        </li>
                                        <li className="list-inline-item">
                                            <a className="text-muted" href="https://adminkit.io/"  rel="noreferrer" target="_blank">Terms</a>
                                        </li>
                                    </ul>
                                </div>
                            </div>
                        </div>
                    </footer>
                </div>
            </div>
            <Script src="/js/site.js" strategy="afterInteractive" />
        </div>
       
    )
}

export default MainLayout