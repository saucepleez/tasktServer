import React from 'react';
import PropTypes from 'prop-types';
import { withStyles } from '@material-ui/core/styles';
import Card from '@material-ui/core/Card';
import CardActions from '@material-ui/core/CardActions';
import CardContent from '@material-ui/core/CardContent';
import Button from '@material-ui/core/Button';
import Typography from '@material-ui/core/Typography';
import '../BotDashboard';
import Loader from '../../Loader';
const styles = {
    card: {
        width: 220,
        height: 100,
        background: 'white',
        display: 'inline-block',
        marginRight: 15,
        marginLeft: 0,
        marginTop: 10,
        marginBottom: 10,
    },
    title: {
        fontSize: 18,
        color: '#3c3c3c',
        fontFamily: 'Ubuntu, sans-serif',
        textAlign: 'center'
    },
    body: {
        fontSize: 20,
        color: 'steelblue',
        fontFamily: 'Ubuntu, sans-serif',
        textAlign: 'center'
    }
};


function MetricCard(props) {

    const { classes } = props;
    var metric = 'default';
    var metricName = 'default';

    if (props.metricName == 'Loading') {
        return (
            <Card className={classes.card} raised='true'>
                <CardContent>
                    <Loader type='spin' color='steelBlue' width='50px' height='50px'></Loader>;             
                </CardContent>
            </Card>);

    }
    else {
        return (

            <Card className={classes.card} raised='true'>
                <CardContent>
                    <Typography className={classes.title} color="textSecondary" gutterBottom>
                        {props.metricName}
                    </Typography>

                    <Typography className={classes.body}>
                        {props.metric}
                    </Typography>

                </CardContent>

            </Card>
        );
    }
   
   
}



MetricCard.propTypes = {
    classes: PropTypes.object.isRequired,
};

export default withStyles(styles)(MetricCard);