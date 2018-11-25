-- phpMyAdmin SQL Dump
-- version 4.8.3
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Nov 25, 2018 at 05:20 PM
-- Server version: 10.1.35-MariaDB
-- PHP Version: 7.2.9

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET AUTOCOMMIT = 0;
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `praktikum_pv`
--

-- --------------------------------------------------------

--
-- Table structure for table `deliveries`
--

CREATE TABLE `deliveries` (
  `id` int(10) UNSIGNED NOT NULL,
  `no` varchar(255) NOT NULL,
  `date` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `destination` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `delivery_details`
--

CREATE TABLE `delivery_details` (
  `id` int(10) UNSIGNED NOT NULL,
  `delivery_id` int(11) UNSIGNED NOT NULL,
  `item_id` int(11) UNSIGNED NOT NULL,
  `qty` int(11) UNSIGNED NOT NULL DEFAULT '0'
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `items`
--

CREATE TABLE `items` (
  `id` int(11) UNSIGNED NOT NULL,
  `name` varchar(255) NOT NULL,
  `category` varchar(255) NOT NULL,
  `unit` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `purchase_orders`
--

CREATE TABLE `purchase_orders` (
  `id` int(10) UNSIGNED NOT NULL,
  `no` varchar(255) NOT NULL,
  `date` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `status` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `purchase_order_details`
--

CREATE TABLE `purchase_order_details` (
  `id` int(10) UNSIGNED NOT NULL,
  `purchase_order_id` int(11) UNSIGNED NOT NULL,
  `item_id` int(11) UNSIGNED NOT NULL,
  `supplier_id` int(11) UNSIGNED NOT NULL,
  `qty` int(11) UNSIGNED NOT NULL DEFAULT '0',
  `qty_complete` int(11) UNSIGNED NOT NULL DEFAULT '0'
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `purchase_receipts`
--

CREATE TABLE `purchase_receipts` (
  `id` int(11) UNSIGNED NOT NULL,
  `no` varchar(255) NOT NULL,
  `date` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `purchase_order_id` int(11) UNSIGNED NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `purchase_receipt_details`
--

CREATE TABLE `purchase_receipt_details` (
  `id` int(10) UNSIGNED NOT NULL,
  `purchase_receipt_id` int(11) UNSIGNED NOT NULL,
  `purchase_order_detail_id` int(11) UNSIGNED NOT NULL,
  `qty` int(10) UNSIGNED NOT NULL DEFAULT '0'
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `stocks`
--

CREATE TABLE `stocks` (
  `id` int(11) UNSIGNED NOT NULL,
  `item_id` int(11) UNSIGNED NOT NULL,
  `qty` int(11) UNSIGNED NOT NULL DEFAULT '0',
  `location` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `suppliers`
--

CREATE TABLE `suppliers` (
  `id` int(11) UNSIGNED NOT NULL,
  `name` varchar(255) NOT NULL,
  `address` text,
  `phone` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `users`
--

CREATE TABLE `users` (
  `id` int(11) NOT NULL,
  `username` varchar(255) NOT NULL,
  `password` varchar(255) NOT NULL,
  `role` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Indexes for dumped tables
--

--
-- Indexes for table `deliveries`
--
ALTER TABLE `deliveries`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `delivery_details`
--
ALTER TABLE `delivery_details`
  ADD PRIMARY KEY (`id`),
  ADD KEY `delivery_id` (`delivery_id`),
  ADD KEY `item_id` (`item_id`);

--
-- Indexes for table `items`
--
ALTER TABLE `items`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `purchase_orders`
--
ALTER TABLE `purchase_orders`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `no` (`no`);

--
-- Indexes for table `purchase_order_details`
--
ALTER TABLE `purchase_order_details`
  ADD PRIMARY KEY (`id`),
  ADD KEY `purchase_order_id` (`purchase_order_id`),
  ADD KEY `item_id` (`item_id`),
  ADD KEY `supplier_id` (`supplier_id`);

--
-- Indexes for table `purchase_receipts`
--
ALTER TABLE `purchase_receipts`
  ADD PRIMARY KEY (`id`),
  ADD KEY `purchase_order_id` (`purchase_order_id`);

--
-- Indexes for table `purchase_receipt_details`
--
ALTER TABLE `purchase_receipt_details`
  ADD PRIMARY KEY (`id`),
  ADD KEY `purchase_receipt_id` (`purchase_receipt_id`),
  ADD KEY `purchase_order_detail_id` (`purchase_order_detail_id`);

--
-- Indexes for table `stocks`
--
ALTER TABLE `stocks`
  ADD PRIMARY KEY (`id`),
  ADD KEY `item_id` (`item_id`);

--
-- Indexes for table `suppliers`
--
ALTER TABLE `suppliers`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `users`
--
ALTER TABLE `users`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `username` (`username`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `deliveries`
--
ALTER TABLE `deliveries`
  MODIFY `id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `delivery_details`
--
ALTER TABLE `delivery_details`
  MODIFY `id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `items`
--
ALTER TABLE `items`
  MODIFY `id` int(11) UNSIGNED NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `purchase_orders`
--
ALTER TABLE `purchase_orders`
  MODIFY `id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `purchase_order_details`
--
ALTER TABLE `purchase_order_details`
  MODIFY `id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `purchase_receipts`
--
ALTER TABLE `purchase_receipts`
  MODIFY `id` int(11) UNSIGNED NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `purchase_receipt_details`
--
ALTER TABLE `purchase_receipt_details`
  MODIFY `id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `stocks`
--
ALTER TABLE `stocks`
  MODIFY `id` int(11) UNSIGNED NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `suppliers`
--
ALTER TABLE `suppliers`
  MODIFY `id` int(11) UNSIGNED NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `users`
--
ALTER TABLE `users`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `delivery_details`
--
ALTER TABLE `delivery_details`
  ADD CONSTRAINT `delivery_details_ibfk_1` FOREIGN KEY (`delivery_id`) REFERENCES `deliveries` (`id`),
  ADD CONSTRAINT `delivery_details_ibfk_2` FOREIGN KEY (`item_id`) REFERENCES `items` (`id`);

--
-- Constraints for table `purchase_order_details`
--
ALTER TABLE `purchase_order_details`
  ADD CONSTRAINT `purchase_order_details_ibfk_1` FOREIGN KEY (`item_id`) REFERENCES `items` (`id`),
  ADD CONSTRAINT `purchase_order_details_ibfk_2` FOREIGN KEY (`purchase_order_id`) REFERENCES `purchase_orders` (`id`),
  ADD CONSTRAINT `purchase_order_details_ibfk_3` FOREIGN KEY (`supplier_id`) REFERENCES `suppliers` (`id`);

--
-- Constraints for table `purchase_receipts`
--
ALTER TABLE `purchase_receipts`
  ADD CONSTRAINT `purchase_receipts_ibfk_1` FOREIGN KEY (`purchase_order_id`) REFERENCES `purchase_orders` (`id`);

--
-- Constraints for table `stocks`
--
ALTER TABLE `stocks`
  ADD CONSTRAINT `stocks_ibfk_1` FOREIGN KEY (`item_id`) REFERENCES `items` (`id`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
