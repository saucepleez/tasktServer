import React, { Component } from 'react';
import ReactDOM from 'react-dom';
import Button from '@material-ui/core/Button';
import AssignmentList from './components/AssignmentList';

export class Assignments extends Component {
    displayName = Assignments.name

    render() {
        return (

            <div>
                <h1>My Assignments</h1>         
                <AssignmentList api="/api/Assignments/All"></AssignmentList>
            </div>
        );
    }
}


