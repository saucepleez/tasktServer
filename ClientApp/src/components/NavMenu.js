import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import { Glyphicon, Nav, Navbar, NavItem } from 'react-bootstrap';
import { LinkContainer } from 'react-router-bootstrap';
import './NavMenu.css';

export class NavMenu extends Component {
    displayName = NavMenu.name

    render() {
        return (
            <Navbar inverse fixedTop fluid collapseOnSelect>
                <Navbar.Header>
                    <Navbar.Brand>
                        <Link to={'/'}>taskt server</Link>
                    </Navbar.Brand>
                    <Navbar.Toggle />
                </Navbar.Header>
                <Navbar.Collapse>
                    <Nav>

                        <LinkContainer to={'/'} exact>
                            <NavItem>
                                <Glyphicon glyph='home' />My Dashboard
                            </NavItem>
                        </LinkContainer>

                        <LinkContainer to={'/workforce'} exact>
                            <NavItem>
                                <Glyphicon glyph='briefcase' />My Workforce
                            </NavItem>
                        </LinkContainer>



                    </Nav>
                </Navbar.Collapse>
            </Navbar>
        );
    }
}
