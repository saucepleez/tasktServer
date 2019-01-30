import React from 'react';
import ReactLoading from 'react-loading';
 
const Loader = ({ type, color, width, height }) => (
    <ReactLoading type={type} color={color} height={width} width={height} />
);
 
export default Loader;