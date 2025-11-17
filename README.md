通过正则表达式进行替换。例如：

*.txt  
N  
---@class ==>> public class  
---@field ==>> public  
-- ==>> //  
\bfunction\b ==>> public void  
\bend\b ==>> }  
\bfor\b ==>> for(  
\bdo\b ==>> ){  
\bif\b ==>> if(  
\bthen\b ==>> ){  
\belse\b ==>> }else{  
\belseif\b ==>> }else_if{  
\blocal\b ==>> var  
: ==>> .  
\bnil\b ==>> null  
\bself\b ==>> this  
\bGetComponent\b ==>> GetComponent<  
\blogRed\b ==>> Debug.LogError  
\bboolean\b ==>> bool  
\bnot\b ==>> !  
\band\b ==>> &&  
\bor\b ==>> ||  


第一行要替换的文件
第二行是否替换文件(Y/N)

