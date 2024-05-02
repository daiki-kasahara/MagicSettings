# Magic Settings

<!-- TOC tocDepth:2..3 chapterDepth:2..6 -->

- [1. 機能概要](#1-機能概要)
- [2. 動作環境](#2-動作環境)
  - [2.1. OS](#21-os)
- [3. 機能詳細](#3-機能詳細)
  - [3.1. キーバインディング機能](#31-キーバインディング機能)
  - [3.2. ブルーライトカット機能](#32-ブルーライトカット機能)
  - [3.3. アプリの設定](#33-アプリの設定)
- [4. 基本設計](#4-基本設計)
- [5. 詳細設計](#5-詳細設計)
- [6. 開発環境](#6-開発環境)
  - [6.1. PC](#61-pc)
  - [6.2. ツール](#62-ツール)

<!-- /TOC -->

## 1. 機能概要

Magic Settings とは、OS の設定にはない便利な機能を提供する Windows アプリケーションである。

## 2. 動作環境

### 2.1. OS

- Windows 10 Version 1809 以上
- Windows 11 Version 21H2 以上

## 3. 機能詳細

### 3.1. キーバインディング機能

キーバインディングとは、特定のキーの組み合わせにアクションを割り当てることである。  
本機能は、設定したキーの組み合わせを押下することで設定したアクションを実行することができる。  
キーの組み合わせは Win キー + Alt キー + 設定したいキーとなる。  
Win キー + Alt キーを固定にしているのは、他のアプリのショートカットとの競合をなるべく避けるためである。
すでに設定したキーの組み合わせが他のアプリで設定されている場合、アクションが実行されないことがある。  
(そのアプリの仕様によって実行されるされないが変わる)
本アプリで設定したキーの組み合わせを押下したとき、本アプリ以外のアプリもキー押下を検出出来るようにしているため、  
本アプリ以外のアクションも実行されることがある。
同じキーの組合せで割り当てられるアクションは一つのみ。  
既に設定されているキーの組み合わせを再度新規で設定しようとした場合はエラーメッセージを表示する。

#### 3.1.1. GUI

#### 3.1.2. 設定できるアクション

| アクション                 | 説明                                               | オプション                                          |
| :------------------------- | :------------------------------------------------- | :-------------------------------------------------- |
| アプリの起動               | オプションで設定したアプリを起動する               | 起動するファイルのパス                              |
| Web ページを表示           | オプションで設定した URL を開く                    | URL (http:または https:から始まる URL のみ設定可能) |
| スクリーンショット         | スクリーンショットを実行する                       |                                                     |
| Snipping Tool の起動       | Snipping Tool を起動する                           |                                                     |
| Microsoft Store を起動     | Microsoft Store を起動する                         |                                                     |
| 設定アプリの起動           | 設定アプリを起動する                               |                                                     |
| Bluetooth デバイスの設定   | 設定アプリの Bluetooth デバイス設定ページを開く    |                                                     |
| カメラの設定               | 設定アプリのカメラ設定ページを開く                 |                                                     |
| マウスの設定               | 設定アプリのマウス設定ページを開く                 |                                                     |
| キーボードの設定           | 設定アプリのキーボード設定ページを開く             |                                                     |
| ネットワークの設定         | 設定アプリのネットワーク設定ページを開く           |                                                     |
| Wi-Fi の設定               | 設定アプリの Wi-Fi 設定ページを開く                |                                                     |
| システム情報を表示         | 設定アプリのシステム情報ページを開く               |                                                     |
| アプリごとのボリューム設定 | 設定アプリのアプリごとのボリューム設定ページを開く |                                                     |
| バッテリー節約機能         | 設定アプリのバッテリー節約機能を開く               |                                                     |
| バッテリー節約機能の設定   | 設定アプリのバッテリー節約機能設定ページを開く     |                                                     |
| バッテリーの使用状況       | 設定アプリのバッテリーの使用状況ページを開く       |                                                     |
| ディスプレイ設定           | 設定アプリのディスプレイ設定ページを開く           |                                                     |
| 集中モードの設定           | 設定アプリの集中モード設定ページを開く             |                                                     |
| ナイトモードの設定         | 設定アプリのナイトモード設定ページを開く           |                                                     |
| 通知の設定                 | 設定アプリの通知設定ページを開く                   |                                                     |
| システムサウンドの設定     | 設定アプリのシステムサウンド設定ページを開く       |                                                     |
| サウンドデバイスの設定     | 設定アプリのサウンドデバイス設定ページを開く       |                                                     |
| ストレージ情報を表示       | 設定アプリのストレージ情報ページを開く             |                                                     |

### 3.2. ブルーライトカット機能

ディスプレイのスクリーンの青色要素を軽減し、目への負担を減らす機能。  
軽減率は 10% ~ 100%の 10%刻みで設定が可能。

#### 3.2.1. GUI

### 3.3. アプリの設定

本ページには、アプリ情報表示機能、アプリのテーマ設定機能、OSS ライセンス表示機能がある。  
OSS ライセンス表示は、ライセンスの条件に従い、各 OSS のライセンス本文を表示している。

#### 3.3.1. GUI

## 4. 基本設計

## 5. 詳細設計

## 6. 開発環境

### 6.1. PC

動作確認も本 PC で実施

- Windows 11 Version 22H2
- CPU Intel
- メモリ 32GB

### 6.2. ツール

- Visual Studion 2022 Community
  - 開発で利用
- Visual Studio Code
  - GIT の GUI 操作で利用
  - ドキュメント作成で利用
- Git
  - ホスティングサービスは Github
  - Ci のために Github Actions を使用
