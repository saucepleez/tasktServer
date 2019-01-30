import React, { Component } from 'react';
import { Col, Grid, Row } from 'react-bootstrap';
import { NavMenu } from './NavMenu';

export class Layout extends Component {
  displayName = Layout.name

  render() {
    return (
      <Grid fluid>
        <Row>
          <Col sm={2}>
            <NavMenu />
          </Col>
          <Col sm={10}>
            {this.props.children}
          </Col>
        </Row>
      </Grid>
    );
  }
}
