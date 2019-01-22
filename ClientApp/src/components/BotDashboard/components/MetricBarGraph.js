import React, { Component } from 'react';
import '../../../../node_modules/react-vis/dist/style.css' 
import { XYPlot, XAxis, YAxis, HorizontalGridLines, VerticalGridLines, LineSeries } from 'react-vis';


export default class MetricBarGraph extends Component {

    render(props) {

        return (

            <XYPlot
                    xType="time"
                    width={200}
                    height={200}>
                <HorizontalGridLines />
                <VerticalGridLines />
                    <LineSeries
                        data={[
                        { x: new Date('1/1/19'), y: 100 },
                        { x: new Date('1/2/19'), y: 200 },
                        { x: new Date('1/3/19'), y: 25}
                        ]} />
                    <XAxis />
                    <YAxis />
            </XYPlot>
         

        );


    }
}
